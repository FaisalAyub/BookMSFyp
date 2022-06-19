import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  isLoggedUser:boolean=false;
  constructor(private authService:AuthService) { 

  }

  ngOnInit(): void {
    this.authService.setAccessToken.subscribe((res)=>{
      if(res.length>0){
        this.isLoggedUser=true;
      }else{
        this.isLoggedUser=false;
      }
    });

    let authToken=localStorage.getItem('authToken');
    if(authToken?.length){
      this.isLoggedUser=true;
    }else{
      this.isLoggedUser=false;
    }
  }

  logout(){
    localStorage.clear();
    location.reload();
  }
}
