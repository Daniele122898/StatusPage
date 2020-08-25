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
  public subEntityForms: FormGroup[];
  @Input() statusConfig: StatusConfig;
  public subEntities: StatusConfig[];

  constructor(
    private fb: FormBuilder,
    private configService: StatusConfigService,
  ) { }

  public hasError(formControl: string): boolean {
    return this.statusConfigForm.get(formControl).hasError('required') && this.statusConfigForm.get(formControl).touched;
  }

  public subHasError(index: number, formControl: string): boolean {
    return this.subEntityForms[index].get(formControl).hasError('required') && this.subEntityForms[index].get(formControl).touched;
  }

  ngOnInit(): void {
    this.createForm();
    if (this.statusConfig && this.statusConfig.isCategory && this.statusConfig.subEntities.length > 0) {
      this.subEntities = this.statusConfig.subEntities;
      this.createSubForms();
    }
    this.populateForm();
  }

  public submit(): void {
    if (!this.statusConfigForm.valid) {
      return;
    }

    if (this.statusConfig) {
      this.editConfig();
    } else {
      this.createConfig();
    }
  }

  public formNotUpdatedOrValid(): boolean {
    return (!this.statusConfigForm.valid && this.statusConfigForm.touched) || !this.statusConfigForm.touched;
  }

  private createConfig(): void {
    const conf: StatusConfig = this.statusConfigForm.value;
    this.configService.createStatusConfig(conf).subscribe(
      () => {
        this.configService.forceListRefresh();
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
        this.configService.forceListRefresh();
      }, err => {
        console.error(err);
        alert(err.error);
      }
    );
  }

  public addSubEntity(): void {
    this.subEntities.push({
      enabled: true,
      healthEndpoint: '',
      identifier: '',
      isCategory: false,
      description: ''
    });

    this.subEntityForms.push(this.fb.group({
      identifier: ['', Validators.required],
      description: [''],
      healthEndpoint: ['', Validators.required],
      enabled: [true, Validators.required]
    }));
  }

  public remove(): void {
    if (!this.statusConfig) {
      return;
    }
    this.configService.removeConfig(this.statusConfig.identifier).subscribe(
      () => {
        this.configService.forceListRefresh();
      }, err => {
        console.error(err);
        alert(err.error);
      }
    );
  }

  public removeSubEntity(index: number): void {

  }

  private createForm(): void {
    this.statusConfigForm = this.fb.group({
      identifier: ['', Validators.required],
      description: [''],
      healthEndpoint: [''],
      enabled: [true, Validators.required],
      isCategory: [false, Validators.required],
    });
  }

  private createSubForms(): void {
    this.subEntityForms = [];
    this.statusConfig.subEntities.forEach((ent) => {
      this.subEntityForms.push(this.fb.group({
        identifier: [ent.identifier, Validators.required],
        description: [(ent.description ? ent.description : '')],
        healthEndpoint: [ent.healthEndpoint, Validators.required],
        enabled: [ent.enabled, Validators.required]
      }));
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
