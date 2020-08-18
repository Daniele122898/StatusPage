import { Component, OnInit } from '@angular/core';
import {ColorSchemeService} from '../../shared/Services/color-scheme.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  constructor(private colorSchemeService: ColorSchemeService) { }

  public onColorSwitch(): void {
    if (this.colorSchemeService.currentActive() === 'light'){
      this.colorSchemeService.update('dark');
    } else {
      this.colorSchemeService.update('light');
    }
  }

}
