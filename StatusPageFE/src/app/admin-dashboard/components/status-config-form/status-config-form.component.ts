import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {StatusConfig} from '../../models/StatusConfig';
import {StatusConfigService} from '../../services/status-config.service';

@Component({
  selector: 'app-status-config-form',
  templateUrl: './status-config-form.component.html',
  styleUrls: ['./status-config-form.component.scss']
})
export class StatusConfigFormComponent implements OnInit {

  public statusConfigForm: FormGroup;
  @Input() statusConfig: StatusConfig;

  constructor(
    private fb: FormBuilder,
    private configService: StatusConfigService,
  ) { }

  public hasError(formControl: string): boolean {
    return this.statusConfigForm.get(formControl).hasError('required') && this.statusConfigForm.get(formControl).touched;
  }

  ngOnInit(): void {
    this.createForm();
    this.populateForm();
  }

  public submit(): void {
    if (!this.statusConfigForm) {
      return;
    }

    if (this.statusConfig) {
      this.editConfig();
    } else {
      this.createConfig();
    }
  }

  private createConfig(): void {
    const conf: StatusConfig = this.statusConfigForm.value;
    this.configService.createStatusConfig(conf).subscribe(
      () => {
        // TODO force list refresh
      }, err => {
        console.error(err);
        alert(err.error);
      }
    );
  }

  private editConfig(): void {
    const conf: StatusConfig = this.statusConfigForm.value;
    this.configService.editStatusConfig(this.statusConfig.identifier, conf).subscribe(
      () => {
        // TODO force list refresh
      }, err => {
        console.error(err);
        alert(err.error);
      }
    );
  }

  public remove(): void {
    if (!this.statusConfig) {
      return;
    }
    this.configService.removeConfig(this.statusConfig.identifier).subscribe(
      () => {
        // TODO force list refresh
      }, err => {
        console.error(err);
        alert(err.error);
      }
    );
  }

  private createForm(): void {
    this.statusConfigForm = this.fb.group({
      identifier: ['', Validators.required],
      description: [''],
      // subEntities: [''], // TODO figure out how to do subentities
      healthEndpoint: ['', Validators.required],
      enabled: [true, Validators.required],
      isCategory: [false, Validators.required],
    });
  }

  private populateForm(): void {
    if (!this.statusConfig) {
      return;
    }
    this.statusConfigForm.get('identifier').setValue(this.statusConfig.identifier);
    this.statusConfigForm.get('description').setValue(this.statusConfig.description);
    this.statusConfigForm.get('healthEndpoint').setValue(this.statusConfig.healthEndpoint);
    this.statusConfigForm.get('enabled').setValue(this.statusConfig.enabled);
    this.statusConfigForm.get('isCategory').setValue(this.statusConfig.isCategory);
  }
}
