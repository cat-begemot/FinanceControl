import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountInfoComponent } from './account-info/account-info.component';
import { AccountEditorComponent } from './account-editor/account-editor.component';

const routes: Routes = [
  {path: '', redirectTo: '/accounts', pathMatch: 'full'},
  {path: 'accounts', component: AccountInfoComponent},
  {path: 'accounts/add', component: AccountEditorComponent},
  {path: 'accounts/edit/:id', component: AccountEditorComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
