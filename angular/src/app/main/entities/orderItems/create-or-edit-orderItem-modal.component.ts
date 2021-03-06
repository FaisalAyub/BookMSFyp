import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OrderItemsServiceProxy, CreateOrEditOrderItemDto ,OrderItemBookLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { OrderItemOrderLookupTableModalComponent } from './orderItem-order-lookup-table-modal.component';



@Component({
    selector: 'createOrEditOrderItemModal',
    templateUrl: './create-or-edit-orderItem-modal.component.html'
})
export class CreateOrEditOrderItemModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderItemOrderLookupTableModal', { static: true }) orderItemOrderLookupTableModal: OrderItemOrderLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderItem: CreateOrEditOrderItemDto = new CreateOrEditOrderItemDto();

    orderOrderName = '';
    bookTitle = '';

	allBooks: OrderItemBookLookupTableDto[];
					

    constructor(
        injector: Injector,
        private _orderItemsServiceProxy: OrderItemsServiceProxy
    ) {
        super(injector);
    }
    
    show(orderItemId?: number): void {
    

        if (!orderItemId) {
            this.orderItem = new CreateOrEditOrderItemDto();
            this.orderItem.id = orderItemId;
            this.orderOrderName = '';
            this.bookTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._orderItemsServiceProxy.getOrderItemForEdit(orderItemId).subscribe(result => {
                this.orderItem = result.orderItem;

                this.orderOrderName = result.orderOrderName;
                this.bookTitle = result.bookTitle;


                this.active = true;
                this.modal.show();
            });
        }
        this._orderItemsServiceProxy.getAllBookForTableDropdown().subscribe(result => {						
						this.allBooks = result;
					});
					
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._orderItemsServiceProxy.createOrEdit(this.orderItem)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrderModal() {
        this.orderItemOrderLookupTableModal.id = this.orderItem.orderId;
        this.orderItemOrderLookupTableModal.displayName = this.orderOrderName;
        this.orderItemOrderLookupTableModal.show();
    }


    setOrderIdNull() {
        this.orderItem.orderId = null;
        this.orderOrderName = '';
    }


    getNewOrderId() {
        this.orderItem.orderId = this.orderItemOrderLookupTableModal.id;
        this.orderOrderName = this.orderItemOrderLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
