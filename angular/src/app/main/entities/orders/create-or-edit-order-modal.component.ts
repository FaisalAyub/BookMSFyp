import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OrdersServiceProxy, CreateOrEditOrderDto ,OrderUserLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditOrderModal',
    templateUrl: './create-or-edit-order-modal.component.html'
})
export class CreateOrEditOrderModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    order: CreateOrEditOrderDto = new CreateOrEditOrderDto();

    userName = '';

	allUsers: OrderUserLookupTableDto[];
					

    constructor(
        injector: Injector,
        private _ordersServiceProxy: OrdersServiceProxy
    ) {
        super(injector);
    }
    
    show(orderId?: number): void {
    

        if (!orderId) {
            this.order = new CreateOrEditOrderDto();
            this.order.id = orderId;
            this.order.orderDate = moment().startOf('day');
            this.userName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._ordersServiceProxy.getOrderForEdit(orderId).subscribe(result => {
                this.order = result.order;

                this.userName = result.userName;


                this.active = true;
                this.modal.show();
            });
        }
        this._ordersServiceProxy.getAllUserForTableDropdown().subscribe(result => {						
						this.allUsers = result;
					});
					
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._ordersServiceProxy.createOrEdit(this.order)
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
