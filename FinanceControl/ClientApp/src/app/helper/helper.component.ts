import { Component, OnInit, Input } from '@angular/core';
import { Helper, Target } from "../model/helper.model";
import { Repository } from "../model/repository";

@Component({
  selector: 'app-helper',
  templateUrl: './helper.component.html',
  styleUrls: ['./helper.component.css']
})
export class HelperComponent implements OnInit {
  @Input()
  id: number;
  
  public helpers: Helper[];
  
  constructor(
    public repository: Repository
  ) { }

  ngOnInit() {
    let target: Target;
    if(this.id==0){
      target=Target.Signin
    }
    else if(this.id==1){
      target=Target.Signup;
    }
    else if(this.id==2){
      target=Target.Accounts;
    }
    else if(this.id==3){
      target=Target.Transactions;
    }
    else if(this.id==4){
      target=Target.Items;
    }
    else if(this.id==5){
      target=Target.Currencies;
    }
    else if(this.id==6){
      target=Target.Groups;
    }

    this.repository.getHelpersByTarget(target).subscribe(res=>
      {
        this.helpers=res;
      });

  }

}
