import { Injectable } from '@angular/core';
import { Repository } from "../model/repository";
import { Router } from "@angular/router";
import { Observable, of } from "rxjs";

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
  ) { 
      // check whether is client authenticated
      this.repository.isAuthenticated().subscribe((response: string)=>{
        if(response=="true"){
          this.authenticated=true;
        } else{
          this.authenticated=false;
        }
      });
  }

  public login(): Observable<boolean>{
    let obs:  Observable<any> = this.repository.login(this.name, this.password);

    obs.subscribe(response=>{
      if(response==true)
      {
        this.authenticated=true;
        this.password=null; 
      }
    });

    return obs;
  }

  public logout(): void{
    this.authenticated=false;
    this.repository.logout().subscribe(()=>{});
  }
}
