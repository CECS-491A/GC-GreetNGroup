import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import PageNotFound from '@/components/PageNotFound'

Vue.use(Router)

/*
  The path '*' is to create a catch all(default) for
  route paths that are unknown (not specified),
  to lead to our 404 page
*/
const router = new Router({
  routes: [
    {
      path: '/',
      name: 'HelloWorld',
      component: HelloWorld
    },
    {
      path: '*',
      component: PageNotFound
    }
  ]
})

export default router
