import { Component, OnInit } from '@angular/core';
import { Repository } from "../../model/repository";
import { Group, GroupType } from "../../model/group.model";

@Component({
  selector: 'app-groups-info',
  templateUrl: './groups-info.component.html',
  styleUrls: ['./groups-info.component.css']
})
export class GroupsInfoComponent implements OnInit {
  public groups: Group[];
  public nameSortAcs: boolean; // sorting direction
  public typeSortAcs: boolean; // sorting direction

  constructor(
    private repository: Repository
  ) { }

  ngOnInit() {
    this.repository.getAllGroup(GroupType.None).subscribe(response=>{
      this.groups=response;
    });
    this.nameSortAcs=false;
    this.typeSortAcs=false;
  }

  public getTypeValue(key: number): string{
    return GroupType[key];
  }

  public click_nameSort(){
    this.nameSortAcs=!this.nameSortAcs;
    this.groups.sort((a: Group, b: Group)=>{
      if(a.name>b.name){
        if(this.nameSortAcs) return -1;
        return 1;
      }
      else if (a.name<b.name){
        if(this.nameSortAcs) return 1;
        return -1;
      }
      else if(a.name=b.name)
        return 0;
    });
  }

  public click_typeSort(){
    this.typeSortAcs=!this.typeSortAcs;
    this.groups.sort((a: Group, b: Group)=>{
      if(a.type>b.type){
        if(this.typeSortAcs) return -1;
        return 1;
      }
      else if (a.type<b.type){
        if(this.typeSortAcs) return 1;
        return -1;
      }
      else if(a.type=b.type)
        return 0;
    });
  }
}
