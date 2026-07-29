import { Routes } from "@angular/router";
import { RegistrationComponent } from "./components/registration/registration.component";
import { LoginComponent } from "./components/login/login.component";
import { LayoutComponent } from "./components/layout/layout.component";
import { AuthGuard } from "../identity/identity-shared/auth.guard";
import { ForcePasswordChangeComponent } from "./components/login/force-password-change/force-password-change.component";
import { TriggerResetPasswordComponent } from "./components/layout/dashboard/master-settings/access-control/triggerResetPassword/triggerResetPassword.component";
import { SetUserAccountUnitComponent } from "./components/login/set-user-account-unit/set-user-account-unit.component";
import { AccessDeniedComponent } from "./shared/access-denied/access-denied.component";
// import { ReportsComponent } from "./components/layout/dashboard/reports/reports.component";

export const routes: Routes = [
  
  // Login component
  { path: "", component: LoginComponent, pathMatch: "full" },
  { path: "login", component: LoginComponent },

  // Register component
  { path: "registration", component: RegistrationComponent },
  { path: "change-password", component: ForcePasswordChangeComponent },
  
  // For set user account unit
  { path: 'set-user-account-unit', component: SetUserAccountUnitComponent },

  // For layout
  {
    path: "app",
    component: LayoutComponent,
    children: [
      { path: "access-denied", component: AccessDeniedComponent },

      // Reset Login
      { path: "reset-Login", component: TriggerResetPasswordComponent }
    ],
    canActivate: [AuthGuard]
  }
];