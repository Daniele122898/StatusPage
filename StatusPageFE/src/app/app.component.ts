import { Component } from '@angular/core';
import {ColorSchemeService} from '../Services/color-scheme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public panelOpenState = false;

  constructor(private colorSchemeService: ColorSchemeService) {
    this.colorSchemeService.load();
  }
}
