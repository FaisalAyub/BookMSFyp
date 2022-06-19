import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './pages/index/index.component';
import { LoginComponent } from './pages/login/login.component';
const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'app/index'
      },
      {
        path:'app/index',
        component:IndexComponent
      },
      {
        path:'account/login',
        component:LoginComponent
      },   
      {
        path: '**',
        component: IndexComponent
        // component: PageNotFoundComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
