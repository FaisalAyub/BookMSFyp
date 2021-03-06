import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { OrderItemsServiceProxy, OrderItemDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { MasterDetailChild_Order_CreateOrEditOrderItemModalComponent } from './masterDetailChild_Order_create-or-edit-orderItem-modal.component';

import { MasterDetailChild_Order_ViewOrderItemModalComponent } from './masterDetailChild_Order_view-orderItem-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './masterDetailChild_Order_orderItems.component.html',
    selector: "masterDetailChild_Order_orderItems-component",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class MasterDetailChild_Order_OrderItemsComponent extends AppComponentBase {
    @Input("orderId") orderId: any;
    
    @ViewChild('createOrEditOrderItemModal', { static: true }) createOrEditOrderItemModal: MasterDetailChild_Order_CreateOrEditOrderItemModalComponent;
    @ViewChild('viewOrderItemModalComponent', { static: true }) viewOrderItemModal: MasterDetailChild_Order_ViewOrderItemModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxQuantityFilter : number;
		maxQuantityFilterEmpty : number;
		minQuantityFilter : number;
		minQuantityFilterEmpty : number;
    maxPriceFilter : number;
		maxPriceFilterEmpty : number;
		minPriceFilter : number;
		minPriceFilterEmpty : number;
        bookTitleFilter = '';




    constructor(
        injector: Injector,
        private _orderItemsServiceProxy: OrderItemsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getOrderItems(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderItemsServiceProxy.getAll(
            this.filterText,
            this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty: this.maxQuantityFilter,
            this.minQuantityFilter == null ? this.minQuantityFilterEmpty: this.minQuantityFilter,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            null,
            this.bookTitleFilter,
            this.orderId,
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

    createOrderItem(): void {
        this.createOrEditOrderItemModal.show(this.orderId);        
    }


    deleteOrderItem(orderItem: OrderItemDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._orderItemsServiceProxy.delete(orderItem.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    
}
