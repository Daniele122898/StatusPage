import { Component } from '@angular/core';
import {ColorSchemeService} from '../shared/Services/color-scheme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private colorSchemeService: ColorSchemeService) {
    this.colorSchemeService.load();
  }
}
