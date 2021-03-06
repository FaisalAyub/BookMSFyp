import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { OrdersComponent } from './entities/orders/orders.component';
import { ViewOrderModalComponent } from './entities/orders/view-order-modal.component';
import { CreateOrEditOrderModalComponent } from './entities/orders/create-or-edit-order-modal.component';

import { MasterDetailChild_Order_OrderItemsComponent } from './entities/orderItems/masterDetailChild_Order_orderItems.component';
import { MasterDetailChild_Order_ViewOrderItemModalComponent } from './entities/orderItems/masterDetailChild_Order_view-orderItem-modal.component';
import { MasterDetailChild_Order_CreateOrEditOrderItemModalComponent } from './entities/orderItems/masterDetailChild_Order_create-or-edit-orderItem-modal.component';

import { OrderItemOrderLookupTableModalComponent } from './entities/orderItems/orderItem-order-lookup-table-modal.component';

import { OrderItemsComponent } from './entities/orderItems/orderItems.component';
import { ViewOrderItemModalComponent } from './entities/orderItems/view-orderItem-modal.component';
import { CreateOrEditOrderItemModalComponent } from './entities/orderItems/create-or-edit-orderItem-modal.component';

import { BooksComponent } from './entities/books/books.component';
import { ViewBookModalComponent } from './entities/books/view-book-modal.component';
import { CreateOrEditBookModalComponent } from './entities/books/create-or-edit-book-modal.component';
import { UserLookupTableModalComponent } from './entities/books/user-lookup-table-modal.component';

import { AutoCompleteModule } from 'primeng/primeng';
import { PaginatorModule } from 'primeng/primeng';
import { EditorModule } from 'primeng/primeng';
import { InputMaskModule } from 'primeng/primeng';import { FileUploadModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';

import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { from } from 'rxjs';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,		TableModule,

        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot()
    ],
    declarations: [
		OrdersComponent,

		ViewOrderModalComponent,
		CreateOrEditOrderModalComponent,
		MasterDetailChild_Order_OrderItemsComponent,

		MasterDetailChild_Order_ViewOrderItemModalComponent,
		MasterDetailChild_Order_CreateOrEditOrderItemModalComponent,
    OrderItemOrderLookupTableModalComponent,
		OrderItemsComponent,

		ViewOrderItemModalComponent,
		CreateOrEditOrderItemModalComponent,
		BooksComponent,
		ViewBookModalComponent,		CreateOrEditBookModalComponent,
    UserLookupTableModalComponent, 
        DashboardComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
        
    ]
})
export class MainModule { }
