﻿using SignalR.SelfHosted.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.SelfHosted.Messages.Services;

public class ConversationService : IConversationService
{
    private readonly IDataContext _context;

    public ConversationService(IDataContext context)
    {
        _context = context;
    }

    public Conversation Create(CreateConversationRequest request)
    {
        var conversation = new Conversation
        {
            Id = _context.Conversations.Count + 1,
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            CreatorUserId = request.CreatorUserId,
        };

        _context.Conversations.Add(conversation);
        return conversation;
    }

    public IEnumerable<ConversationModel> GetAll()
    {
        return from c in _context.Conversations
               join cu in _context.Users on c.CreatorUserId equals cu.Id
               select new ConversationModel
               {
                   Id = c.Id,
                   Title = c.Title,
                   CreatedAt = c.CreatedAt,
                   CreatorUser = new Users.Models.UserModel
                   {
                       Id = cu.Id,
                       FullName = cu.FullName,
                       OnLine = cu.OnLine,
                       PhotoUrl = cu.PhotoUrl
                   }
               };
    }

    public IEnumerable<ConversationAudienceModel> GetAudiences(int conversationId)
    {
        var audiences = from ca in _context.ConversationAudiences.Where(x => x.ConversationId == conversationId)
                        join au in _context.Users on ca.AudienceUserId equals au.Id
                        join c in _context.Conversations on ca.ConversationId equals c.Id
                        join cu in _context.Users on c.CreatorUserId equals cu.Id
                        select new ConversationAudienceModel
                        {
                            AudienceUser = new Users.Models.UserModel
                            {
                                Id = au.Id,
                                OnLine = au.OnLine,
                                FullName = au.FullName,
                                PhotoUrl = au.PhotoUrl,
                            },
                            Conversation = new ConversationModel
                            {
                                Id = c.Id,
                                CreatedAt = c.CreatedAt,
                                Title = c.Title,
                                CreatorUser = new Users.Models.UserModel
                                {
                                    Id = cu.Id,
                                    FullName = cu.FullName,
                                    OnLine = cu.OnLine,
                                    PhotoUrl = cu.PhotoUrl,
                                }
                            }
                        };

        return audiences;
    }

    public IEnumerable<MessageModel> GetMessages(int conversationId)
    {
        var messages = from m in _context.Messages.Where(x => x.ConversationId == conversationId)
                       join c in _context.Conversations on m.ConversationId equals c.Id
                       join cu in _context.Users on c.CreatorUserId equals cu.Id
                       select new MessageModel
                       {
                           Id = m.Id,
                           Text = m.Text,
                           CreatedAt = m.CreatedAt,
                           Conversation = new ConversationModel
                           {
                               Id = c.Id,
                               CreatedAt = c.CreatedAt,
                               Title = c.Title,
                               CreatorUser = new Users.Models.UserModel
                               {
                                   Id = cu.Id,
                                   FullName = cu.FullName,
                                   OnLine = cu.OnLine,
                                   PhotoUrl = cu.PhotoUrl,
                               }
                           }
                       };
        return messages;
    }
}