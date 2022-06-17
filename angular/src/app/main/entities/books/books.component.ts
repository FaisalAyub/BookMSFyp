import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router'; 
import { BooksServiceProxy, BookDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBookModalComponent } from './create-or-edit-book-modal.component';
import { ViewBookModalComponent } from './view-book-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './books.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BooksComponent extends AppComponentBase {

    @ViewChild('createOrEditBookModal', {static: true}) createOrEditBookModal: CreateOrEditBookModalComponent;
    @ViewChild('viewBookModalComponent', {static: true}) viewBookModal: ViewBookModalComponent;
    @ViewChild('entityTypeHistoryModal', {static: true}) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', {static: true}) dataTable: Table;
    @ViewChild('paginator', {static: true}) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    isbnFilter = '';
    authorFilter = '';
    descriptionFilter = '';
    publisherFilter = '';
    maxPriceFilter : number;
		maxPriceFilterEmpty : number;
		minPriceFilter : number;
		minPriceFilterEmpty : number;
    maxQuantityFilter : number;
		maxQuantityFilterEmpty : number;
		minQuantityFilter : number;
		minQuantityFilterEmpty : number;
        userNameFilter = '';


    _entityTypeFullName = 'ERP.Entities.Book';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _booksServiceProxy: BooksServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
    }

    getBooks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._booksServiceProxy.getAll(
            this.filterText,
            this.titleFilter,
            this.isbnFilter,
            this.authorFilter,
            this.descriptionFilter,
            this.publisherFilter,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty: this.maxQuantityFilter,
            this.minQuantityFilter == null ? this.minQuantityFilterEmpty: this.minQuantityFilter,
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

    createBook(): void {
        this.createOrEditBookModal.show();
    }

    showHistory(book: BookDto): void {
        this.entityTypeHistoryModal.show({
            entityId: book.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteBook(book: BookDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._booksServiceProxy.delete(book.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._booksServiceProxy.getBooksToExcel(
        this.filterText,
            this.titleFilter,
            this.isbnFilter,
            this.authorFilter,
            this.descriptionFilter,
            this.publisherFilter,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty: this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty: this.minPriceFilter,
            this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty: this.maxQuantityFilter,
            this.minQuantityFilter == null ? this.minQuantityFilterEmpty: this.minQuantityFilter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
