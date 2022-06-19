import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { AuthenticateModel, TokenAuthServiceProxy } from 'src/app/shared/service-proxies/service-proxies';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  submitting:boolean=false;
  authenticateModel:any={userNameOrEmailAddress:'',password:'',rememberClient:false,singleSignIn:false,returnUrl:null};
  constructor( private _tokenAuthService: TokenAuthServiceProxy,private _router:Router,private authService:AuthService) { }

  ngOnInit(): void {
  }

  login(): void {
    
    this.submitting = true;
    this._tokenAuthService.authenticate(this.authenticateModel).subscribe((res)=>{
      console.log(res);
      let authenticateResult=res;
      if (authenticateResult.accessToken) {
        // Successfully logged in 
        localStorage.setItem('authToken',authenticateResult.accessToken);
        this._router.navigate(['/app/index']);
        this.authService.setAccessToken.next(authenticateResult.accessToken);
    } else {
        // Unexpected result!

        // this._logService.warn('Unexpected authenticateResult!');
        this._router.navigate(['account/login']);

    }
    })
}

}
