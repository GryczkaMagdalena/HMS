import {Redirect, NavigationInstruction, RouterConfiguration, Router, Next} from 'aurelia-router';
import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {LoadHandlerService} from "./services/load-handler-service";

@inject(EventAggregator, LoadHandlerService)
export class App {

  loggedIn: boolean;
  router: Router;

  constructor(public eventAggregator: EventAggregator, public loadHandlerService: LoadHandlerService) {
  }

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'HMS';
    config.map([
      {
        route: ['', 'login'],
        moduleId: 'views/login/login',
        title: 'Login',
        name: 'login',
        settings: {
          publicOnly: true,
          clientOnly: false,
          employeeOnly: false,
        }
      },
      {
        route: 'client/base',
        moduleId: 'views/client/base/base',
        title: 'Hello Guest!',
        name: 'base',
        settings: {
          auth: true,
          clientOnly: true,
        }
      },
      {
        route: 'client/hotel-rules',
        moduleId: 'views/client/hotel-rules/hotel-rules',
        title: 'Hotel Rules',
        name: 'hotelRules',
        settings: {
          auth: true,
          clientOnly: true,
        }
      },
      {
        route: 'client/cases',
        moduleId: 'views/client/cases/cases',
        title: 'Cases',
        name: 'cases',
        settings: {
          auth: true,
          clientOnly: true,
        }
      },
      {
        route: 'employee/main-panel',
        moduleId: 'views/employee/main-panel/main-panel',
        title: 'Employee Main Panel',
        name: 'employeeMainPanel',
        settings: {
          auth: true,
          employeeOnly: true,
        }
      },
    ]);
    config.addPipelineStep('authorize', AuthorizeStep);

    this.router = router;
    this.loggedIn = false;
  }

  attached() {
    this.eventAggregator.subscribe('login::loggedIn', (data) => {
      AuthorizeStep.auth.isClient = false;
      AuthorizeStep.auth.isEmployee = false;

      AuthorizeStep.auth.isAuthenticated = data.loggedIn;
      if (data.workerType == 'None') {
        AuthorizeStep.auth.isClient = true;
      } else if (data.workerType == 'Technician' || data.workerType === 'Cleaner'){
        AuthorizeStep.auth.isEmployee = true;
      }
    });
  }

}


class AuthorizeStep {

  static auth = {
    isAuthenticated: !!sessionStorage.getItem('session_token'),
    isClient: sessionStorage.getItem('worker_type') == 'None',
    isEmployee: (sessionStorage.getItem('worker_type') == 'Cleaner') || (sessionStorage.getItem('worker_type') == 'Technician'),
  };

  run(navigationInstruction: NavigationInstruction, next: Next) {
    let isLoggedIn = AuthorizeStep.auth.isAuthenticated;
    let isClient = AuthorizeStep.auth.isClient;
    let isEmployee = AuthorizeStep.auth.isEmployee;

    // currently active route config
    let currentRoute = navigationInstruction.config;

    // settings object will be preserved during navigation
    //if currentRoute is auth=true and user isLoggedIn=false -> redirect to the login
    let loginRequired = currentRoute.settings && currentRoute.settings.auth === true;
    if (isLoggedIn === false && loginRequired === true) {
      return next.cancel(new Redirect('login'));
    }

    //if currentRoute is publicOnly=true (means login-view) -> redirect to the employee view or client view
    let publicOnly = currentRoute.settings && currentRoute.settings.publicOnly === true;
    if (isLoggedIn === true && publicOnly === true) {
      if (isClient) {
        return next.cancel(new Redirect('client/base'));
      } else if (isEmployee) {
        return next.cancel(new Redirect('employee/main-panel'));
      }
    }

    // if currentRoute is employeeOnly -> redirect to the client view
    let employeeOnly = currentRoute.settings && currentRoute.settings.employeeOnly === true;
    if (isClient && (isLoggedIn === true) && (employeeOnly === true)) {
      return next.cancel(new Redirect('client/base'));
    }

    // if currentRoute is clientOnly and user is logged as employee -> redirect to the employee view
    let clientOnly = currentRoute.settings && currentRoute.settings.clientOnly === true;
    if (isEmployee && (isLoggedIn === true) && (clientOnly === true)) {
      return next.cancel(new Redirect('employee/main-panel'));
    }

    return next();
  };

}
