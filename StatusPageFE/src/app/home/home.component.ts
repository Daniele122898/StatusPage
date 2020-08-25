import {Component, HostListener, OnInit} from '@angular/core';
import {StatusService} from './services/status.service';
import {ServiceStatus} from '../../shared/models/ServiceStatus';
import {getStatusText} from '../../shared/models/Status';
import {SpecialNotice} from '../../shared/models/SpecialNotice';
import {Router} from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public serviceStatuses: ServiceStatus[];
  public specialNotice: SpecialNotice;

  public getStatusText = getStatusText;

  @HostListener('document:keydown', ['$event']) onKeyDown(e: KeyboardEvent): void {
    if (e.shiftKey && e.code === 'KeyL') {
      this.router.navigate(['/login']);
    }
  }

  constructor(
    private statusService: StatusService,
    private router: Router
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

    this.statusService.getSpecialNotice().subscribe(
      notice => {
        this.specialNotice = notice;
      }, err => {
        if (err.error.status !== 404) {
          console.error(err);
        }
      }
    );
  }
}
