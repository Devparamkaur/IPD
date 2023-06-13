import { ActiveItem } from "./enums";
import { BreadcrumbItem } from "./models";

/* App Globals */
export class Globals {
  public static MiniNav: boolean = true;
  public static ActiveItem: ActiveItem = ActiveItem.Home;
  public static Breadcrumbs: BreadcrumbItem[] = [
    { Id: ActiveItem.Root, Name: "Root" },
    { Id: ActiveItem.Home, Name: "home", ParentId: ActiveItem.Root, Path: '/home' }
  ];
}
