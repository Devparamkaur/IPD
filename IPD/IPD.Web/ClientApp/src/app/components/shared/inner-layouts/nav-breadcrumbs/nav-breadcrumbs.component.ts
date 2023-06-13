import { Component, Input, OnInit } from '@angular/core';
import { BreadcrumbItem } from '../../../../common/models';

@Component({
  selector: 'app-nav-breadcrumbs',
  templateUrl: './nav-breadcrumbs.component.html',
  styleUrls: ['./nav-breadcrumbs.component.css']
})
export class NavBreadcrumbsComponent implements OnInit {

  @Input() model: BreadcrumbItem[] = [];
  constructor() { }

  ngOnInit(): void {
  }


}
