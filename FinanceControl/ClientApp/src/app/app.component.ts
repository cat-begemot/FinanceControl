import { Component } from '@angular/core';
import { AuthenticationService } from './auth/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(
    private authService: AuthenticationService
  ) { }

  public click_Logout(){
    if(this.authService.authenticated==true){
      this.authService.logout();
    }
  }
}
