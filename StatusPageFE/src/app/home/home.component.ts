import { Component, OnInit } from '@angular/core';
import {StatusService} from './services/status.service';
import {ServiceStatus} from '../../shared/models/ServiceStatus';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public serviceStatuses: ServiceStatus[];

  constructor(
    private statusService: StatusService
  ) { }

  ngOnInit(): void {
    this.getAllStatuses();
  }

  private getAllStatuses(): void {
    this.statusService.getServiceStatuses().subscribe(
      statuses => {
        this.serviceStatuses = statuses;
      },
      err => {
        console.error(err);
      }
    );
  }
}
