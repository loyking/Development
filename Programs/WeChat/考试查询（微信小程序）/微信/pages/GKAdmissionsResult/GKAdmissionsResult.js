const app = getApp();

Page({
  data:{
    Batch:"",
    Category:"",
    SchoolName:""
  },
  onLoad:function(option){
    this.setData({
      Batch: option.Batch,
      Category: option.Category,
      SchoolName: option.SchoolName
    })
  },
  ContinueQuery:function(){
    wx.navigateTo({
      url: '../GKQuery/GKQuery',
    })
  },
  Index:function(){
    wx.navigateTo({
      url: '../index/index',
    })
  }
})