import {Component, Input, OnInit} from '@angular/core';
import {ServiceStatus, isServiceCategory} from '../../../../shared/models/ServiceStatus';
import {getStatusColor, getStatusIcon, getStatusText, Status} from '../../../../shared/models/Status';

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
    return getStatusColor(this.serviceStatus.status);
  }

  public getStatusIcon(): string {
    return getStatusIcon(this.serviceStatus.status);
  }

  public getStatusText(): string {
    return getStatusText(this.serviceStatus.status);
  }

}
