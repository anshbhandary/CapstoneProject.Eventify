import { HttpInterceptorFn } from '@angular/common/http';

export const customInterceptor: HttpInterceptorFn = (req, next) => {

  const token = localStorage.getItem('myLogInToken'); 
 const clonereq = req.clone({ 
 setHeaders: { 
 Authorization: `Bearer ${token}`
 } 
 }) 


  return next(clonereq);
};
