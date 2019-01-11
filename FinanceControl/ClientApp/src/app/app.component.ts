import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './auth/authentication.service';
import { Router } from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    public authService: AuthenticationService,
    private routerNav: Router
  ) { }

  public click_Logout(){
    if(this.authService.authenticated==true){
      this.authService.logout();
    }
  }

  ngOnInit(){
    if(this.authService.authenticated==true){
      this.routerNav.navigate(["/accounts"]);
    }
  }
}
