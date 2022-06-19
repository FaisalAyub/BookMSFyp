import { Component, OnInit } from '@angular/core';
import { BooksServiceProxy, GetBookForViewDto } from 'src/app/shared/service-proxies/service-proxies';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  advancedFiltersAreShown = false;
  filterText = '';
  titleFilter = '';
  isbnFilter = '';
  authorFilter = '';
  descriptionFilter = '';
  publisherFilter = '';
  maxPriceFilter : number=0;
  maxPriceFilterEmpty : number=0;
  minPriceFilter : number=0;
  minPriceFilterEmpty : number=0;
  maxQuantityFilter : number=0;
  maxQuantityFilterEmpty : number=0;
  minQuantityFilter : number=0;
  minQuantityFilterEmpty : number=0;
      userNameFilter = '';

      bookItems:GetBookForViewDto[]=[];
  constructor(private _booksServiceProxy: BooksServiceProxy) { }

  ngOnInit(): void {
    this.getBooks()
  }

  getBooks() {
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
        this.userNameFilter,null,0,100
    ).subscribe(result => {
      
      if(result?.items?.length){
        this.bookItems=result.items;
      }
      console.log(this.bookItems);
    });
}


slideConfig = {"slidesToShow": 4, "slidesToScroll": 4};
  
 
 

slickInit(e:any) {
  console.log('slick initialized');
}

breakpoint(e:any) {
  console.log('breakpoint');
}

afterChange(e:any) {
  console.log('afterChange');
}

beforeChange(e:any) {
  console.log('beforeChange');
}


}
