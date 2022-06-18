import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OrderItemsServiceProxy, CreateOrEditOrderItemDto ,OrderItemBookLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'masterDetailChild_Order_createOrEditOrderItemModal',
    templateUrl: './masterDetailChild_Order_create-or-edit-orderItem-modal.component.html'
})
export class MasterDetailChild_Order_CreateOrEditOrderItemModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderItem: CreateOrEditOrderItemDto = new CreateOrEditOrderItemDto();

    bookTitle = '';

	allBooks: OrderItemBookLookupTableDto[];
					

    constructor(
        injector: Injector,
        private _orderItemsServiceProxy: OrderItemsServiceProxy
    ) {
        super(injector);
    }
    
                 orderId: any;
             
    show(
                 orderId: any,
             orderItemId?: number): void {
    
                 this.orderId = orderId;
             

        if (!orderItemId) {
            this.orderItem = new CreateOrEditOrderItemDto();
            this.orderItem.id = orderItemId;
            this.bookTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._orderItemsServiceProxy.getOrderItemForEdit(orderItemId).subscribe(result => {
                this.orderItem = result.orderItem;

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
            
			
			
                this.orderItem.orderId = this.orderId;
            
            this._orderItemsServiceProxy.createOrEdit(this.orderItem)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }













    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
