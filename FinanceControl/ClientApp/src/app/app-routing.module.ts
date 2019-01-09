import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountInfoComponent } from './account-info/account-info.component';
import { AccountEditorComponent } from './account-info/account-editor/account-editor.component';
import { CurrencyInfoComponent } from './account-info/currency-info/currency-info.component';
import { CurrencyEditorComponent } from './account-info/currency-info/currency-editor/currency-editor.component';
import { AuthenticationComponent } from "./auth/authentication/authentication.component";

const routes: Routes = [
  {path: '', redirectTo: '/accounts', pathMatch: 'full'},
  {path: 'accounts/add', component: AccountEditorComponent},
  {path: 'accounts', component: AccountInfoComponent},
  {path: 'accounts', component: AccountInfoComponent},
  {path: 'accounts/edit/:id', component: AccountEditorComponent},
  {path: 'currencies', component: CurrencyInfoComponent},
  {path: 'currencies/add', component: CurrencyEditorComponent},
  {path: 'currencies/edit/:id', component: CurrencyEditorComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
