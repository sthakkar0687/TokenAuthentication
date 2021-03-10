import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  isLoggedIn: boolean = false;
  title = 'app';
  ngOnInit(): void {
    //alert('loaded');
    if (localStorage.getItem('token') != null && localStorage.getItem('userId') != null && localStorage.getItem('email') != null) 
      this.isLoggedIn = true;    
    else
      this.isLoggedIn = false;    
  }
  
   
}
