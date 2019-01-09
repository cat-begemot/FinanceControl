import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from "../authentication.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Repository } from 'src/app/model/repository';
@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {
  public loginForm: FormGroup;

  constructor(
    private authService: AuthenticationService,
    private repository: Repository
  ) { }

  ngOnInit() {
    this.loginForm=new FormGroup({
      loginControl: new FormControl('user', Validators.required),
      passwordControl: new FormControl('user123', Validators.required)
    });


  }

  public onSubmit(){
    this.authService.name=this.loginForm.value["loginControl"];
    this.authService.password=this.loginForm.value["passwordControl"];
    
    if (!this.authService.authenticated)
      this.authService.login();
  }

  public click_Logout(){
    if(this.authService.authenticated)
      this.authService.logout();
  }

  // test function
  public click_GetUser(){    
    this.repository.getUser().subscribe(response=>{
      console.info(response);
    });
  }
}
