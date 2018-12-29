import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountInfoComponent } from './account-info/account-info.component';
import { AccountEditorComponent } from './account-editor/account-editor.component';

const routes: Routes = [
  {path: 'accounts', component: AccountInfoComponent},
  {path: 'accounts/add', component: AccountEditorComponent},
  {path: '', redirectTo: '/accounts', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
