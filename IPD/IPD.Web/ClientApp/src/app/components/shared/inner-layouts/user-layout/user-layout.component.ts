import { ChangeDetectorRef, Component } from '@angular/core';
import { Helpers } from '../../../../common/helpers';

@Component({
  selector: 'app-user-layout',
  templateUrl: './user-layout.component.html',
  styleUrls: ['./user-layout.component.css'],
  animations: [
  ]
})
export class UserLayoutComponent  {
  miniNav: boolean = false;

  get MiniNav(): boolean {
    return this.miniNav;
  }

  set MiniNav(miniNav: boolean) {
    this.miniNav = miniNav;
  }

  constructor(public helpers: Helpers,
    private cdr: ChangeDetectorRef) {
  }

  ngOnInit() {
   
  }

  ngAfterViewChecked() {
    //your code to update the model
    this.cdr.detectChanges();
  }
}
