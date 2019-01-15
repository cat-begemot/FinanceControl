import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountInfoComponent } from './account-info/account-info.component';
import { AccountEditorComponent } from './account-info/account-editor/account-editor.component';
import { CurrencyInfoComponent } from './account-info/currency-info/currency-info.component';
import { CurrencyEditorComponent } from './account-info/currency-info/currency-editor/currency-editor.component';
import { AuthenticationComponent } from "./auth/authentication/authentication.component";
import { SignupComponent } from "./auth/signup/signup.component";
import { GroupEditorComponent} from "./items-info/groups-info/group-editor/group-editor.component";
import { GroupsInfoComponent } from "./items-info/groups-info/groups-info.component";

const routes: Routes = [
  {path: '', redirectTo: '/accounts', pathMatch: 'full'},
  
  {path: 'login', component: AuthenticationComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'accounts', component: AccountInfoComponent},
  {path: 'accounts/add', component: AccountEditorComponent},
  {path: 'accounts/edit/:id', component: AccountEditorComponent},
  {path: 'currencies', component: CurrencyInfoComponent},
  {path: 'currencies/add', component: CurrencyEditorComponent},
  {path: 'currencies/edit/:id', component: CurrencyEditorComponent},
  {path: 'groups', component: GroupsInfoComponent},
  {path: 'groups/add', component: GroupEditorComponent},
  {path: 'groups/edit/:id', component: GroupEditorComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
