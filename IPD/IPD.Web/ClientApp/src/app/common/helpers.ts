import { Injectable } from '@angular/core';
import { Constants } from './constants';
import { ActiveItem } from './enums';
import { BreadcrumbItem } from './models';

/* App Helpers */
@Injectable()
export class Helpers {

  constructor() { }

  getBreadcrumbs(activeItem: ActiveItem): BreadcrumbItem[] {
    let itemsToReturn: BreadcrumbItem[] = [];
    let items: BreadcrumbItem[] = Constants.BREADCRUMBS;
    let item = items.filter(x => x.Id == activeItem)[0];
    if (typeof (item) != typeof (undefined)) {
      item.Active = true;
      do {
        itemsToReturn.push(item);
        item = items.filter(x => x.Id == item.ParentId)[0];
      }
      while (typeof (item) != typeof (undefined) && item.Id != ActiveItem.Root)
    }
    return itemsToReturn.reverse();
  }

  //Generates Id
  generateId() {
    return (Math.random() + 1).toString(36).substring(7);
  }


  //Converts string to UUID
  stringToUuid = (str: any) => {
    let g = str.replace("-", "");
    g = g.substring(0, 32);
    if (/^[0-9A-F]{32}$/i.test(g)) {
      g = g.replace(/(.{8})(.{4})(.{4})(.{4})(.{12})/, "$1-$2-$3-$4-$5");
    }

    return g;
  }

}
