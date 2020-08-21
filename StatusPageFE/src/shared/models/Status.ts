import {ServiceStatus} from './ServiceStatus';

export enum Status {
  Healthy,
  Outage,
  PartialOutage
}

const getStatusColor = (status: Status): string => {
  switch (status) {
    case Status.Healthy:
      return '#388e3c';
    case Status.Outage:
      return '#f44336';
    case Status.PartialOutage:
      return '#ff9800';
  }
};

const getStatusIcon = (status: Status): string => {
  switch (status) {
    case Status.Healthy:
      return 'check_circle';
    case Status.Outage:
      return 'clear';
    case Status.PartialOutage:
      return 'warning';
  }
};

const getStatusText = (status: Status): string => {
  switch (status) {
    case Status.Healthy:
      return 'Healthy';
    case Status.Outage:
      return 'Outage';
    case Status.PartialOutage:
      return 'Partial Outage';
  }
};

export {getStatusColor, getStatusIcon, getStatusText};
