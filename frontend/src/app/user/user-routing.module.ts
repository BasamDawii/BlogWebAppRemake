import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {UserDashboardComponent} from "./components/user-dashboard/user-dashboard.component";
import {RoleGuard} from "../account-management/account-guards/role.guard";
import {UserHomeComponent} from "./components/user-home/user-home.component";
import {MyBlogsComponent} from "./components/my-blogs/my-blogs.component";
import {CreateBlogComponent} from "./components/create-blog/create-blog.component";

const routes: Routes = [
  {
    path: '',
    component: UserDashboardComponent,
    canActivate: [RoleGuard],
    data: {expectedRole: 'User'},
    children: [
      {
        path: "user-home",
        component: UserHomeComponent
      },
      {
        path: "my-blogs",
        component: MyBlogsComponent
      },
      {
        path: "create-blog",
        component: CreateBlogComponent
      },
    ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }

