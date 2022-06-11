import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BooksComponent } from './entities/books/books.component'; 
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'entities/books', component: BooksComponent, data: { permission: 'Pages.Books' }  },
                    // { path: 'accountGroup/glacgrp', component: GLACGRPComponent, data: { permission: 'Pages.GLACGRP' }  },
                    // { path: 'glCostCenter/glCstCent', component: GLCstCentComponent, data: { permission: 'Pages.GLCstCent' }  },
                    // { path: 'books/glbooks', component: GLBOOKSComponent, data: { permission: 'Pages.GLBOOKS' }  },
                    // { path: 'sourceCode/glsrce', component: GLSRCEComponent, data: { permission: 'Pages.GLSRCE' }  },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
