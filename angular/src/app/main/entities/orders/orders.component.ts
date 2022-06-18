﻿import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { OrdersServiceProxy, OrderDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderModalComponent } from './create-or-edit-order-modal.component';

import { ViewOrderModalComponent } from './view-order-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './orders.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OrdersComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditOrderModal', { static: true }) createOrEditOrderModal: CreateOrEditOrderModalComponent;
    @ViewChild('viewOrderModalComponent', { static: true }) viewOrderModal: ViewOrderModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxOrderDateFilter : moment.Moment;
		minOrderDateFilter : moment.Moment;
    descriptionFilter = '';
    statusFilter = '';
    maxTotalBillFilter : number;
		maxTotalBillFilterEmpty : number;
		minTotalBillFilter : number;
		minTotalBillFilterEmpty : number;
    orderNameFilter = '';
        userNameFilter = '';




            orderItemRowSelection: boolean[] = [];
            

                   childEntitySelection: {} = {};
            

    constructor(
        injector: Injector,
        private _ordersServiceProxy: OrdersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getOrders(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._ordersServiceProxy.getAll(
            this.filterText,
            this.maxOrderDateFilter === undefined ? this.maxOrderDateFilter : moment(this.maxOrderDateFilter).endOf('day'),
            this.minOrderDateFilter === undefined ? this.minOrderDateFilter : moment(this.minOrderDateFilter).startOf('day'),
            this.descriptionFilter,
            this.statusFilter,
            this.maxTotalBillFilter == null ? this.maxTotalBillFilterEmpty: this.maxTotalBillFilter,
            this.minTotalBillFilter == null ? this.minTotalBillFilterEmpty: this.minTotalBillFilter,
            this.orderNameFilter,
            this.userNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createOrder(): void {
        this.createOrEditOrderModal.show();        
    }


    deleteOrder(order: OrderDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._ordersServiceProxy.delete(order.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    
    
                  selectEditTable(table){
                      this.childEntitySelection = {};
                      this.childEntitySelection[table] = true;
                  }
            
    
               openChildRowForOrderItem(index, table) {
                   let isAlreadyOpened = this.orderItemRowSelection[index];                   
                   this.closeAllChildRows();                   
                   this.orderItemRowSelection = [];
                   if (!isAlreadyOpened) {
                       this.orderItemRowSelection[index] = true;
                   }
                   this.selectEditTable(table);
               }
            
    
                  closeAllChildRows() : void{
                     
                this.orderItemRowSelection = [];
            
                  }
    
}
