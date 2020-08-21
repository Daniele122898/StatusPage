import {Component, Input, OnInit} from '@angular/core';
import {ServiceStatus, isServiceCategory} from '../../../../shared/models/ServiceStatus';
import {Status} from '../../../../shared/models/Status';

@Component({
  selector: 'app-status-container',
  templateUrl: './status-container.component.html',
  styleUrls: ['./status-container.component.scss']
})
export class StatusContainerComponent implements OnInit {

  @Input() serviceStatus: ServiceStatus;

  public panelOpenState = false;


  constructor() { }

  ngOnInit(): void {
  }

  public isCategory(): boolean {
    return isServiceCategory(this.serviceStatus);
  }

  public getStatusColor(): string {
    switch (this.serviceStatus.status) {
      case Status.Healthy:
        return '#388e3c';
      case Status.Outage:
        return '#f44336';
      case Status.PartialOutage:
        return '#ff9800';
    }
  }

  public getStatusIcon(): string {
    switch (this.serviceStatus.status) {
      case Status.Healthy:
        return 'check_circle';
      case Status.Outage:
        return 'clear';
      case Status.PartialOutage:
        return 'warning';
    }
  }

  public getStatusText(): string {
    switch (this.serviceStatus.status) {
      case Status.Healthy:
        return 'Healthy';
      case Status.Outage:
        return 'Outage';
      case Status.PartialOutage:
        return 'Partial Outage';
    }
  }

}
