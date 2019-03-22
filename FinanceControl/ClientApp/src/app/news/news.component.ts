import { Component, OnInit } from '@angular/core';
import { Info } from "../model/info.model";
import { Repository } from "../model/repository";

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
  public infos: Info[];

  constructor(
    public repository: Repository
  ) { }

  ngOnInit() {
    this.repository.getAllInfos().subscribe(response=>{
      this.infos=response;
    });
  }

}
