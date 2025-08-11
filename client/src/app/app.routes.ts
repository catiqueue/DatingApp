import { Routes } from '@angular/router';
import { Home } from './home/home';
import { UserList } from './users/user-list/user-list';
import { UserDetail } from './users/user-detail/user-detail';
import { Lists } from './lists/lists';
import { Messages } from './messages/messages';
import { authGuard } from './_guards/auth.guard';
import { TestErrors } from './errors/test-errors/test-errors';
import { NotFound } from './errors/not-found/not-found';
import { ServerError } from './errors/server-error/server-error';

export const routes: Routes = [
  {path: "", component: Home},
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [authGuard],
    children: [
      {path: "users", component: UserList},
      {path: "users/:username", component: UserDetail},
      {path: "lists", component: Lists},
      {path: "messages", component: Messages},
    ]
  },
  {path: "errors", component: TestErrors},
  {path: "not-found", component: NotFound},
  {path: "server-error", component: ServerError},
  {path: "**", component: Home, pathMatch: 'full'},
];
