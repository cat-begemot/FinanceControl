import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { ReactiveFormsModule } from "@angular/forms";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AccountInfoComponent } from './account-info/account-info.component';
import { AccountEditorComponent } from './account-info/account-editor/account-editor.component';
import { CurrencyEditorComponent } from './account-info/currency-info/currency-editor/currency-editor.component';
import { CurrencyInfoComponent } from './account-info/currency-info/currency-info.component';
import { AuthenticationComponent } from './auth/authentication/authentication.component';
import { SignupComponent } from './auth/signup/signup.component';
import { ItemsInfoComponent } from './items-info/items-info.component';
import { ItemEditorComponent } from './items-info/item-editor/item-editor.component';
import { GroupsInfoComponent } from './items-info/groups-info/groups-info.component';
import { GroupEditorComponent } from './items-info/groups-info/group-editor/group-editor.component';
import { TransactionsInfoComponent } from './transactions-info/transactions-info.component';
import { TransactionEditComponent } from './transactions-info/transaction-edit/transaction-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    AccountInfoComponent,
    AccountEditorComponent,
    CurrencyEditorComponent,
    CurrencyInfoComponent,
    AuthenticationComponent,
    SignupComponent,
    ItemsInfoComponent,
    ItemEditorComponent,
    GroupsInfoComponent,
    GroupEditorComponent,
    TransactionsInfoComponent,
    TransactionEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
