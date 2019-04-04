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
  targetComponent: Target;
  
  public helpers: Helper[];
  
  constructor(
    public repository: Repository
  ) { }

  ngOnInit() {
    if(this.targetComponent!=Target.None){
      this.repository.getHelpersByTarget(this.targetComponent).subscribe(res=>
        {
          this.helpers=res;
          console.error(this.targetComponent);
        });
    }
  }

}
