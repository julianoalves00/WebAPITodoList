﻿using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Interfaces;

namespace ToDoList.Core.Entities
{
    public class ToDoNote : BaseEntity
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public List<string> Items { get; set; }
    }
}
