import { Injectable } from '@angular/core';
import { Repository } from "../model/repository";
import { Router } from "@angular/router";
import { Observable } from "rxjs"



@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public name: string;
  public password: string;
  public callBackUrl: string;
  public authenticated: boolean;

  constructor(
    private repository: Repository,
    private router: Router
  ) { }

  public login(): boolean{
    this.authenticated=false;
    this.repository.login(this.name, this.password).subscribe(response=>{
      if(response==true)
      {
        this.authenticated=true;
        this.password=null;
        //this.router.navigateByUrl(this.callBackUrl || "/accounts");
      }
    });
    return this.authenticated;
  }

  public logout(): void{
    this.authenticated=false;
    this.repository.logout().subscribe(()=>{});
    //this.router.navigateByUrl("/login");
  }
}
