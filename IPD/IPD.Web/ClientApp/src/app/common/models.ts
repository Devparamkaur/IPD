import { Constants } from "./constants";
import { ActiveItem } from "./enums";
import { Globals } from "./globals";
import { Helpers } from "./helpers";

/* App Models */
export class RootComponent {

  get Breadcrumbs(): BreadcrumbItem[] {
    return Globals.Breadcrumbs;
  }

  get AppTitle(): string {
    return Constants.TITLE;
  }

  updateOuterItems(activeItem: ActiveItem) {
  
    Globals.Breadcrumbs = this.helpers.getBreadcrumbs(activeItem);
  }

  constructor(public helpers: Helpers) { }
}

export interface BreadcrumbItem {
  Id: number;
  ParentId?: number;
  Name: string;
  Active?: boolean;
  Path?: string;
}

