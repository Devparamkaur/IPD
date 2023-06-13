import { ActiveItem } from "./enums";
import { BreadcrumbItem } from "./models";

/* App Constants */
export class Constants {
  public static get LOCAL_STATE(): string { return "local_state"; };
  public static get TITLE(): string { return "IPD"; }
  public static get APP_TITLE(): string { return " | " + this.TITLE; }
  public static get BREADCRUMBS(): BreadcrumbItem[] {
    return [
      { Id: ActiveItem.Root, Name: "Root", Path: '' },
      { Id: ActiveItem.Home, Name: "home", ParentId: ActiveItem.Root, Path: '/home' },
    ];
  };
}
