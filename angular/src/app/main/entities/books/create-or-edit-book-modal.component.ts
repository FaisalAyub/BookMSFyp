import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { BooksServiceProxy, CreateOrEditBookDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { UserLookupTableModalComponent } from './user-lookup-table-modal.component';


@Component({
    selector: 'createOrEditBookModal',
    templateUrl: './create-or-edit-book-modal.component.html'
})
export class CreateOrEditBookModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', {static: true}) modal: ModalDirective;
    // @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    book: CreateOrEditBookDto = new CreateOrEditBookDto();

    userName = '';


    constructor(
        injector: Injector,
        private _booksServiceProxy: BooksServiceProxy
    ) {
        super(injector);
    }

    show(bookId?: number): void {

        if (!bookId) {
            this.book = new CreateOrEditBookDto();
            this.book.id = bookId;
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._booksServiceProxy.getBookForEdit(bookId).subscribe(result => {
                this.book = result.book;

                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._booksServiceProxy.createOrEdit(this.book)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    //     openSelectUserModal() {
    //     this.userLookupTableModal.id = this.book.ownerId;
    //     this.userLookupTableModal.displayName = this.userName;
    //     this.userLookupTableModal.show();
    // }


    //     setOwnerIdNull() {
    //     this.book.ownerId = null;
    //     this.userName = '';
    // }


    //     getNewOwnerId() {
    //     this.book.ownerId = this.userLookupTableModal.id;
    //     this.userName = this.userLookupTableModal.displayName;
    // }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
