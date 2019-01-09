import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppStatusService {
  public activeAccountsMode: boolean = true; // if true - app shows active accounts, else - it shows hidden ones

  constructor() { }
}
