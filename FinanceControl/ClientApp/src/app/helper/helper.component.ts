import { Component, OnInit } from '@angular/core';
import { Helper, Target } from "../model/helper.model";

@Component({
  selector: 'app-helper',
  templateUrl: './helper.component.html',
  styleUrls: ['./helper.component.css']
})
export class HelperComponent implements OnInit {
  public helpers: Helper[];

  constructor() { }

  ngOnInit() {
    this.helpers=[
      new Helper(Target.Signin, "Что здесь?", "На этой странице Вы можете авторизироваться для доступа к своему аккаунту.<br>Учетная запись необходима для авторизации и аутентификации Вас как пользователя сервиса."),
      new Helper(Target.Signin, "Как зарегистрироваться?", "Если у Вас нет учетной записи, нажмите на кнопку <i class=\"fas fa-user-plus mr-1\"></i><b>Registration</b> и следуйте дальнейшим инструкциям."),
      new Helper(Target.Signin, "Как получить доступ к своему аккаунту?", "Введите свой логин и пароль в соответствующие поля и нажмите кнопку <i class=\"fas fa-sign-in-alt mr-1\"></i><b>Log in</b>")
      
      //this.helpers[0]=new Helper(Target.Signin, "", "")
    ];

    
  }

}
