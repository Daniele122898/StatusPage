import { Component, OnInit } from '@angular/core';
import {StatusConfig} from '../../models/StatusConfig';
import {StatusConfigService} from '../../services/status-config.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {

  public statusConfigs: StatusConfig[];

  constructor(
    private configService: StatusConfigService,
  ) { }

  ngOnInit(): void {
    this.configService.getStatusConfigs().subscribe(
      resp => {
        this.statusConfigs = resp;
      }, err => {
        console.error(err);
      }
    );
  }

}
